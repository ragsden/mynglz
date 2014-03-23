
CREATE TABLE [dbo].[UserInfo](
	[UserID] [int] NOT NULL,
	[Handle] [nvarchar](60) NULL,
	[Email] [nvarchar](140) NULL,
	[InstantTitle] [varchar](140) NULL,
	[PictureLocation] [varchar](8000) NULL,
	[OnlineStatusID] [smallint] NOT NULL DEFAULT(0),
	[UpdatedDtTm] [datetime] NULL DEFAULT(GETUTCDATE()),
	[UpdatedBy] [varchar](80) NULL,
	[Gender] smallint NULL,
	[Preference] smallint NULL,
	[Age] [smallint] NOT NULL,
	[CreatedDtTm] [datetime] NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] [varchar](20)  NULL,
 CONSTRAINT [XPKUserInfo] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)
)

