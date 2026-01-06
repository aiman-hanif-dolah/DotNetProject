-- SQLite schema and seed data matching the MVC/MAUI domain
PRAGMA foreign_keys = ON;

DROP TABLE IF EXISTS Quotes;
DROP TABLE IF EXISTS Characters;
DROP TABLE IF EXISTS Episodes;
DROP TABLE IF EXISTS Locations;

CREATE TABLE Characters (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    ActorName TEXT NOT NULL,
    Description TEXT,
    Occupation TEXT,
    ImageUrl TEXT,
    VideoUrl TEXT
);

CREATE TABLE Episodes (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL,
    Season INTEGER NOT NULL,
    EpisodeNumber INTEGER NOT NULL,
    AirDate TEXT,
    Description TEXT,
    ImageUrl TEXT,
    VideoUrl TEXT
);

CREATE TABLE Locations (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Type TEXT NOT NULL,
    Description TEXT,
    Address TEXT,
    ImageUrl TEXT,
    VideoUrl TEXT
);

CREATE TABLE Quotes (
    Id INTEGER PRIMARY KEY,
    Text TEXT NOT NULL,
    Context TEXT,
    CharacterId INTEGER NOT NULL,
    EpisodeId INTEGER NOT NULL,
    FOREIGN KEY (CharacterId) REFERENCES Characters(Id) ON DELETE RESTRICT,
    FOREIGN KEY (EpisodeId) REFERENCES Episodes(Id) ON DELETE RESTRICT
);

INSERT INTO Characters (Id, Name, ActorName, Occupation, Description, ImageUrl, VideoUrl) VALUES
(1, 'Rachel Green', 'Jennifer Aniston', 'Fashion Executive', 'Initially a waitress at Central Perk, later becomes a fashion executive at Ralph Lauren.', '/img/friends/rachel.jpg', 'https://www.youtube.com/embed?listType=search&list=Rachel%20Green%20Friends%20best%20moments'),
(2, 'Monica Geller', 'Courteney Cox', 'Head Chef', 'A perfectionist chef known for her cleanliness obsession and competitive nature.', '/img/friends/monica.jpg', 'https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20Friends%20kitchen%20scenes'),
(3, 'Phoebe Buffay', 'Lisa Kudrow', 'Masseuse/Musician', 'A quirky masseuse and musician known for her song ''Smelly Cat''.', '/img/friends/phoebe.jpg', 'https://www.youtube.com/embed?listType=search&list=Phoebe%20Buffay%20Smelly%20Cat'),
(4, 'Joey Tribbiani', 'Matt LeBlanc', 'Actor', 'A lovable but dim-witted aspiring actor best known for his role as Dr. Drake Ramoray.', '/img/friends/joey.jpg', 'https://www.youtube.com/embed?listType=search&list=Joey%20Tribbiani%20how%20you%20doin'),
(5, 'Chandler Bing', 'Matthew Perry', 'IT Procurement/Advertising', 'Known for his sarcastic wit and comedic timing. Later transitions to advertising.', '/img/friends/chandler.jpg', 'https://www.youtube.com/embed?listType=search&list=Chandler%20Bing%20sarcasm'),
(6, 'Ross Geller', 'David Schwimmer', 'Paleontologist', 'Monica''s older brother, a paleontologist who has been married three times.', '/img/friends/ross.jpg', 'https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20pivot%20scene');

INSERT INTO Episodes (Id, Title, Season, EpisodeNumber, AirDate, Description, ImageUrl, VideoUrl) VALUES
(1, 'The One Where Monica Gets a Roommate', 1, 1, '1994-09-22', 'Rachel runs away from her wedding and moves in with Monica.', '/img/friends/ep-s01e01.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20S01E01%20pilot%20clip'),
(2, 'The One with the Sonogram at the End', 1, 2, '1994-09-29', 'Ross finds out his ex-wife Carol is pregnant with his baby.', '/img/friends/ep-s01e02.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20sonogram%20episode'),
(3, 'The One with the Blackout', 1, 7, '1994-11-03', 'A power outage in New York City leads to adventures for the group.', '/img/friends/ep-s01e07.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20blackout%20episode'),
(4, 'The One Where Ross Finds Out', 2, 7, '1995-11-09', 'Ross discovers Rachel''s feelings for him through a drunken voicemail.', '/img/friends/ep-s02e07.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20Ross%20finds%20out%20episode'),
(5, 'The One with the Prom Video', 2, 14, '1996-02-01', 'The gang watches Rachel and Monica''s prom video, revealing Ross''s long-time love for Rachel.', '/img/friends/ep-s02e14.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20prom%20video%20episode'),
(6, 'The One Where No One''s Ready', 3, 2, '1996-09-26', 'Ross struggles to get everyone ready for a museum event.', '/img/friends/ep-s03e02.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20no%20ones%20ready%20episode');

INSERT INTO Locations (Id, Name, Type, Description, Address, ImageUrl, VideoUrl) VALUES
(1, 'Central Perk', 'Coffee Shop', 'The iconic coffee house where the friends spend most of their time.', '90 Bedford Street, New York', '/img/friends/location-central-perk.jpg', 'https://www.youtube.com/embed?listType=search&list=Central%20Perk%20set%20tour'),
(2, 'Monica''s Apartment', 'Apartment', 'Monica''s purple-walled apartment where most scenes take place.', '90 Bedford Street, Apt 20, New York', '/img/friends/location-monicas-apartment.jpg', 'https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20apartment%20tour'),
(3, 'Joey & Chandler''s Apartment', 'Apartment', 'The apartment across the hall from Monica''s, featuring the famous foosball table.', '90 Bedford Street, Apt 19, New York', '/img/friends/location-joey-chandler-apartment.jpg', 'https://www.youtube.com/embed?listType=search&list=Joey%20and%20Chandler%20apartment%20tour'),
(4, 'Ross''s Apartment', 'Apartment', 'Ross''s various apartments throughout the series.', 'New York', '/img/friends/location-ross-apartment.jpg', 'https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20apartment%20tour'),
(5, 'Museum of Prehistoric History', 'Workplace', 'Where Ross works as a paleontologist.', 'New York', '/img/friends/location-museum.jpg', 'https://www.youtube.com/embed?listType=search&list=Friends%20museum%20set%20tour');

INSERT INTO Quotes (Id, Text, Context, CharacterId, EpisodeId) VALUES
(1, 'We were on a break!', 'Ross''s famous catchphrase about his relationship with Rachel', 6, 1),
(2, 'How you doin''?', 'Joey''s signature pickup line', 4, 1),
(3, 'Could this BE any more awkward?', 'Chandler''s sarcastic observation', 5, 1),
(4, 'Oh. My. God!', 'Janice''s iconic catchphrase, often imitated by others', 2, 2),
(5, 'See, he''s her lobster!', 'Phoebe explaining Ross and Rachel''s relationship', 3, 5),
(6, 'PIVOT! PIVOT! PIVOT!', 'Ross trying to move a couch up the stairs', 6, 6);
