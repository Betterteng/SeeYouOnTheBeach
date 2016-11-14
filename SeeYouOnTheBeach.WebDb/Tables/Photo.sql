CREATE TABLE [dbo].[Photo]
(
	[PhotoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BeachId] INT NOT NULL,
	[Description] NVARCHAR(MAX),
	[Content] VARBINARY(MAX) NOT NULL,
	CONSTRAINT FK_Photo_to_Beach FOREIGN KEY([BeachId]) REFERENCES [Beach]([BeachId])
)

