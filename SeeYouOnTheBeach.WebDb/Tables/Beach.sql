CREATE TABLE [dbo].[Beach]
(
	[BeachId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BeachName] nvarchar(20),
	[Description] NVARCHAR(MAX),
	[Latitude] NUMERIC(18,14),
	[Longitude] Numeric(18,14)
)
