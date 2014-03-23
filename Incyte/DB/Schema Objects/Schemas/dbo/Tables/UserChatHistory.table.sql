CREATE TABLE [dbo].[UserChatHistory](
	ChatId	BIGINT	NOT NULL,
	[HigherUserID] [int] NOT NULL,
	[LowerUserID] [int] NOT NULL,
	[IsHigherMessage] BIT NOT NULL DEFAULT(0),
	[UserMessage] NVARCHAR(120) NOT NULL,
	[CreatedDtTm] [datetime] NOT NULL DEFAULT(GETUTCDATE())
)
