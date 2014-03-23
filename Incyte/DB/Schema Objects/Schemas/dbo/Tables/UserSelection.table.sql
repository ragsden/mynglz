CREATE TABLE [dbo].[UserSelection](
	FromUserID				INT NOT NULL,
	ToUserId				INT	NOT NULL,
	StatusId				SMALLINT	NOT NULL,
	UpdatedDtTm				DATETIME	NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [XPKUserSelection] PRIMARY KEY NONCLUSTERED 
(
	FromUserID,
	ToUserId
)
)

