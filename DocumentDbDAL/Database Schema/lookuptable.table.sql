USE [DocumentDb]
GO

/****** Object:  Table [dbo].[LookUpTable]    Script Date: 2/5/2022 8:55:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LookUpTable](
	[id] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LookUpTable] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LookUpTable] ADD  CONSTRAINT [DF_LookUpTable_id]  DEFAULT (newid()) FOR [id]
GO
 
/****** Object:  Index [IXUniqueContent]    Script Date: 2/5/2022 8:55:51 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IXUniqueContent] ON [dbo].[LookUpTable]
(
	[Category] ASC,
	[Name] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
 


