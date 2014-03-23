CREATE TABLE [dbo].[UserChat](
	ChatId	BIGINT	NOT NULL IDENTITY(1,1),
	[HigherUserID] [int] NOT NULL,
	[LowerUserID] [int] NOT NULL,
	[IsHigherMessage] BIT NOT NULL DEFAULT(0),
	[UserMessage] NVARCHAR(120) NOT NULL,
	[CreatedDtTm] [datetime] NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [XPKUserChat] PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC
)
)
