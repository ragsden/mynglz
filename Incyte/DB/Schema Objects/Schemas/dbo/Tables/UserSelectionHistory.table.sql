CREATE TABLE [dbo].[UserSelectionHistory](
	UserCheckInId			INT NOT NULL,
	FromUserID				INT NOT NULL,
	ToUserId				INT	NOT NULL,
	StatusId				SMALLINT	NOT NULL,
	UpdatedDtTm				DATETIME	NOT NULL DEFAULT(GETUTCDATE()),
)
