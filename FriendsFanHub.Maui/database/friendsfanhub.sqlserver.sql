-- SQL Server schema and seed data matching the MVC site
SET NOCOUNT ON;

IF OBJECT_ID(N'[dbo].[Quotes]', 'U') IS NOT NULL DROP TABLE [dbo].[Quotes];
IF OBJECT_ID(N'[dbo].[Characters]', 'U') IS NOT NULL DROP TABLE [dbo].[Characters];
IF OBJECT_ID(N'[dbo].[Episodes]', 'U') IS NOT NULL DROP TABLE [dbo].[Episodes];
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL DROP TABLE [dbo].[Locations];

CREATE TABLE [dbo].[Characters](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [ActorName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Occupation] NVARCHAR(100) NULL,
    [ImageUrl] NVARCHAR(500) NULL,
    [VideoUrl] NVARCHAR(500) NULL
);

CREATE TABLE [dbo].[Episodes](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(200) NOT NULL,
    [Season] INT NOT NULL,
    [EpisodeNumber] INT NOT NULL,
    [AirDate] DATE NULL,
    [Description] NVARCHAR(1000) NULL,
    [ImageUrl] NVARCHAR(500) NULL,
    [VideoUrl] NVARCHAR(500) NULL
);

CREATE TABLE [dbo].[Locations](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Type] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Address] NVARCHAR(300) NULL,
    [ImageUrl] NVARCHAR(500) NULL,
    [VideoUrl] NVARCHAR(500) NULL
);

CREATE TABLE [dbo].[Quotes](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Text] NVARCHAR(500) NOT NULL,
    [Context] NVARCHAR(500) NULL,
    [CharacterId] INT NOT NULL,
    [EpisodeId] INT NOT NULL,
    CONSTRAINT [FK_Quotes_Characters] FOREIGN KEY([CharacterId]) REFERENCES [dbo].[Characters]([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Quotes_Episodes] FOREIGN KEY([EpisodeId]) REFERENCES [dbo].[Episodes]([Id]) ON DELETE NO ACTION
);

SET IDENTITY_INSERT [dbo].[Characters] ON;
INSERT INTO [dbo].[Characters] (Id, Name, ActorName, Occupation, Description, ImageUrl, VideoUrl) VALUES
(1, N'Rachel Green', N'Jennifer Aniston', N'Fashion Executive', N'Initially a waitress at Central Perk, later becomes a fashion executive at Ralph Lauren.', N'/img/friends/rachel.jpg', N'https://www.youtube.com/embed?listType=search&list=Rachel%20Green%20Friends%20best%20moments'),
(2, N'Monica Geller', N'Courteney Cox', N'Head Chef', N'A perfectionist chef known for her cleanliness obsession and competitive nature.', N'/img/friends/monica.jpg', N'https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20Friends%20kitchen%20scenes'),
(3, N'Phoebe Buffay', N'Lisa Kudrow', N'Masseuse/Musician', N'A quirky masseuse and musician known for her song ''Smelly Cat''.', N'/img/friends/phoebe.jpg', N'https://www.youtube.com/embed?listType=search&list=Phoebe%20Buffay%20Smelly%20Cat'),
(4, N'Joey Tribbiani', N'Matt LeBlanc', N'Actor', N'A lovable but dim-witted aspiring actor best known for his role as Dr. Drake Ramoray.', N'/img/friends/joey.jpg', N'https://www.youtube.com/embed?listType=search&list=Joey%20Tribbiani%20how%20you%20doin'),
(5, N'Chandler Bing', N'Matthew Perry', N'IT Procurement/Advertising', N'Known for his sarcastic wit and comedic timing. Later transitions to advertising.', N'/img/friends/chandler.jpg', N'https://www.youtube.com/embed?listType=search&list=Chandler%20Bing%20sarcasm'),
(6, N'Ross Geller', N'David Schwimmer', N'Paleontologist', N'Monica''s older brother, a paleontologist who has been married three times.', N'/img/friends/ross.jpg', N'https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20pivot%20scene');
SET IDENTITY_INSERT [dbo].[Characters] OFF;

SET IDENTITY_INSERT [dbo].[Episodes] ON;
INSERT INTO [dbo].[Episodes] (Id, Title, Season, EpisodeNumber, AirDate, Description, ImageUrl, VideoUrl) VALUES
(1, N'The One Where Monica Gets a Roommate', 1, 1, '1994-09-22', N'Rachel runs away from her wedding and moves in with Monica.', N'/img/friends/ep-s01e01.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20S01E01%20pilot%20clip'),
(2, N'The One with the Sonogram at the End', 1, 2, '1994-09-29', N'Ross finds out his ex-wife Carol is pregnant with his baby.', N'/img/friends/ep-s01e02.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20sonogram%20episode'),
(3, N'The One with the Blackout', 1, 7, '1994-11-03', N'A power outage in New York City leads to adventures for the group.', N'/img/friends/ep-s01e07.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20blackout%20episode'),
(4, N'The One Where Ross Finds Out', 2, 7, '1995-11-09', N'Ross discovers Rachel''s feelings for him through a drunken voicemail.', N'/img/friends/ep-s02e07.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20Ross%20finds%20out%20episode'),
(5, N'The One with the Prom Video', 2, 14, '1996-02-01', N'The gang watches Rachel and Monica''s prom video, revealing Ross''s long-time love for Rachel.', N'/img/friends/ep-s02e14.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20prom%20video%20episode'),
(6, N'The One Where No One''s Ready', 3, 2, '1996-09-26', N'Ross struggles to get everyone ready for a museum event.', N'/img/friends/ep-s03e02.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20no%20ones%20ready%20episode');
SET IDENTITY_INSERT [dbo].[Episodes] OFF;

SET IDENTITY_INSERT [dbo].[Locations] ON;
INSERT INTO [dbo].[Locations] (Id, Name, Type, Description, Address, ImageUrl, VideoUrl) VALUES
(1, N'Central Perk', N'Coffee Shop', N'The iconic coffee house where the friends spend most of their time.', N'90 Bedford Street, New York', N'/img/friends/location-central-perk.jpg', N'https://www.youtube.com/embed?listType=search&list=Central%20Perk%20set%20tour'),
(2, N'Monica''s Apartment', N'Apartment', N'Monica''s purple-walled apartment where most scenes take place.', N'90 Bedford Street, Apt 20, New York', N'/img/friends/location-monicas-apartment.jpg', N'https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20apartment%20tour'),
(3, N'Joey & Chandler''s Apartment', N'Apartment', N'The apartment across the hall from Monica''s, featuring the famous foosball table.', N'90 Bedford Street, Apt 19, New York', N'/img/friends/location-joey-chandler-apartment.jpg', N'https://www.youtube.com/embed?listType=search&list=Joey%20and%20Chandler%20apartment%20tour'),
(4, N'Ross''s Apartment', N'Apartment', N'Ross''s various apartments throughout the series.', N'New York', N'/img/friends/location-ross-apartment.jpg', N'https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20apartment%20tour'),
(5, N'Museum of Prehistoric History', N'Workplace', N'Where Ross works as a paleontologist.', N'New York', N'/img/friends/location-museum.jpg', N'https://www.youtube.com/embed?listType=search&list=Friends%20museum%20set%20tour');
SET IDENTITY_INSERT [dbo].[Locations] OFF;

SET IDENTITY_INSERT [dbo].[Quotes] ON;
INSERT INTO [dbo].[Quotes] (Id, Text, Context, CharacterId, EpisodeId) VALUES
(1, N'We were on a break!', N'Ross''s famous catchphrase about his relationship with Rachel', 6, 1),
(2, N'How you doin''?', N'Joey''s signature pickup line', 4, 1),
(3, N'Could this BE any more awkward?', N'Chandler''s sarcastic observation', 5, 1),
(4, N'Oh. My. God!', N'Janice''s iconic catchphrase, often imitated by others', 2, 2),
(5, N'See, he''s her lobster!', N'Phoebe explaining Ross and Rachel''s relationship', 3, 5),
(6, N'PIVOT! PIVOT! PIVOT!', N'Ross trying to move a couch up the stairs', 6, 6);
SET IDENTITY_INSERT [dbo].[Quotes] OFF;
