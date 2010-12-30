CREATE TABLE [dbo].[PrFActionLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Milliseconds] [float] NULL,
	[ActionKey] [varchar](100) NOT NULL,
	[UserKey] [varchar](100) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_ActionLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[PrFActionLogParameter](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActionLogID] [int] NOT NULL,
	[Key] [varchar](50) NOT NULL,
	[Values] [text] NOT NULL,
 CONSTRAINT [PK_ActionLogParameter] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[PrFActionLogParameter]  WITH CHECK ADD  CONSTRAINT [FK_PrFActionLogParameter_PrFActionLog] FOREIGN KEY([ActionLogID])
REFERENCES [dbo].[PrFActionLog] ([ID])

ALTER TABLE [dbo].[PrFActionLogParameter] CHECK CONSTRAINT [FK_PrFActionLogParameter_PrFActionLog]

CREATE TABLE [dbo].[PrFExceptionLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](100) NOT NULL,
	[Message] [text] NOT NULL,
	[StackTrace] [text] NOT NULL,
	[InnerMessage] [text] NULL,
	[InnerStackTrace] [text] NULL,
	[Guid] [varchar](50) NOT NULL,
	[ActionLogID] [int] NOT NULL,
 CONSTRAINT [PK_ExceptionLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[PrFExceptionLog]  WITH CHECK ADD  CONSTRAINT [FK_PrFExceptionLog_PrFActionLog] FOREIGN KEY([ActionLogID])
REFERENCES [dbo].[PrFActionLog] ([ID])

ALTER TABLE [dbo].[PrFExceptionLog] CHECK CONSTRAINT [FK_PrFExceptionLog_PrFActionLog]
