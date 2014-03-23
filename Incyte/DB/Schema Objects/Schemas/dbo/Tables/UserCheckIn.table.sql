CREATE TABLE [dbo].[UserCheckIn](
	[UserID] [int] NOT NULL,
	[StartTime] [datetime] NULL ,
	CheckinCount INT DEFAULT(0),
	[UpdatedDtTm] [datetime] NOT NULL DEFAULT(GETUTCDATE()),
	[BusinessId] [int] NULL,
 CONSTRAINT [XPKBusinessCheckIn] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)
)
