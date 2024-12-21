USE [Practice]
GO

/****** Object:  Table [dbo].[order]    Script Date: 20.12.2024 16:18:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[order](
	[id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[product_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[price] [numeric](18, 2) NOT NULL,
	[count] [int] NOT NULL,
	[sum] [numeric](18, 2) NOT NULL,
	[date_order] [datetime] NULL,
 CONSTRAINT [PK_order] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[product] ([id])
GO

ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_product]
GO

ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_user] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
GO

ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_user]
GO


