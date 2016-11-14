CREATE TABLE [dbo].[BeachFeature]
(
	[BeachFeatureId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BeachId] INT NOT NULL,
	[BeachFilterId] INT NOT NULL, 
    CONSTRAINT [FK_BeachFeature_ToBeach] FOREIGN KEY ([BeachId]) REFERENCES [Beach]([BeachId]),
	CONSTRAINT [FK_BeachFeature_ToBeachFilter] FOREIGN KEY ([BeachFilterId]) REFERENCES [BeachFilter](BeachFilterId)
)
