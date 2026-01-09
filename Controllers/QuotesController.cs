using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetProject.Data;
using DotNetProject.Models;
using DotNetProject.Services;

namespace DotNetProject.Controllers
{
    public class QuotesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FirestoreSyncService _firestore;

        public QuotesController(AppDbContext context, FirestoreSyncService firestore)
        {
            _context = context;
            _firestore = firestore;
        }

        // GET: Quotes
        public async Task<IActionResult> Index(string searchString, int? characterId)
        {
            var quotesQuery = _context.Quotes
                .Include(q => q.Character)
                .Include(q => q.Episode)
                .AsQueryable();

            // Filter by search keyword
            if (!string.IsNullOrEmpty(searchString))
            {
                quotesQuery = quotesQuery.Where(q =>
                    q.Text.Contains(searchString) ||
                    (q.Context != null && q.Context.Contains(searchString)));
            }

            // Filter by character
            if (characterId.HasValue)
            {
                quotesQuery = quotesQuery.Where(q => q.CharacterId == characterId.Value);
            }

            // Populate character dropdown for filter
            ViewData["Characters"] = new SelectList(_context.Characters, "Id", "Name", characterId);
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCharacter"] = characterId;

            return View(await quotesQuery.ToListAsync());
        }

        // GET: Quotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await _context.Quotes
                .Include(q => q.Character)
                .Include(q => q.Episode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // GET: Quotes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CharacterId"] = new SelectList(await _context.Characters.ToListAsync(), "Id", "Name");
            ViewData["EpisodeId"] = new SelectList(
                (await _context.Episodes.ToListAsync()).Select(e => new { e.Id, Display = $"S{e.Season:D2}E{e.EpisodeNumber:D2} - {e.Title}" }),
                "Id", "Display");
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Context,CharacterId,EpisodeId")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CharacterId"] = new SelectList(await _context.Characters.ToListAsync(), "Id", "Name", quote.CharacterId);
            ViewData["EpisodeId"] = new SelectList(
                (await _context.Episodes.ToListAsync()).Select(e => new { e.Id, Display = $"S{e.Season:D2}E{e.EpisodeNumber:D2} - {e.Title}" }),
                "Id", "Display", quote.EpisodeId);
            return View(quote);
        }

        // GET: Quotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            ViewData["CharacterId"] = new SelectList(await _context.Characters.ToListAsync(), "Id", "Name", quote.CharacterId);
            ViewData["EpisodeId"] = new SelectList(
                (await _context.Episodes.ToListAsync()).Select(e => new { e.Id, Display = $"S{e.Season:D2}E{e.EpisodeNumber:D2} - {e.Title}" }),
                "Id", "Display", quote.EpisodeId);
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Context,CharacterId,EpisodeId")] Quote quote)
        {
            if (id != quote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuoteExists(quote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CharacterId"] = new SelectList(await _context.Characters.ToListAsync(), "Id", "Name", quote.CharacterId);
            ViewData["EpisodeId"] = new SelectList(
                (await _context.Episodes.ToListAsync()).Select(e => new { e.Id, Display = $"S{e.Season:D2}E{e.EpisodeNumber:D2} - {e.Title}" }),
                "Id", "Display", quote.EpisodeId);
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await _context.Quotes
                .Include(q => q.Character)
                .Include(q => q.Episode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();
                await _firestore.DeleteAsync("quotes", id);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool QuoteExists(int id)
        {
            return _context.Quotes.Any(e => e.Id == id);
        }
    }
}
