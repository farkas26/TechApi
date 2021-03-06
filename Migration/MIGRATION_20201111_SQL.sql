CREATE DATABASE BPM_SIE;
GO
USE [BPM_SIE]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 11/11/2020 18:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[number] [nchar](10) NULL,
	[birthdate] [datetime] NULL,
	[email] [varchar](50) NULL,
	[token] [varchar](100) NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 11/11/2020 18:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SKU] [varchar](50) NULL,
	[nombre] [varchar](50) NOT NULL,
	[cantidad] [varchar](50) NOT NULL,
	[precio] [decimal](10, 2) NOT NULL,
	[descripcion] [varchar](100) NULL,
	[imagen] [varbinary](max) NULL,
 CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Informacion inicial para pruebas ******/
SET IDENTITY_INSERT [dbo].[Login] ON 

INSERT [dbo].[Login] ([id], [name], [username], [password], [number], [birthdate], [email], [token]) VALUES (1, NULL, N'admin', N'adminpass', N'77212234  ', NULL, N'franciscocruz2610@gmail.com', N'55b170aefd72d85088ad503819ccfb26543c160cc6047313ce2c8b31a350e0a7')
INSERT [dbo].[Login] ([id], [name], [username], [password], [number], [birthdate], [email], [token]) VALUES (3, N'francisco', N'farkas', N'1234', N'1234      ', CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'franciscocruz@gmail.com', NULL)
SET IDENTITY_INSERT [dbo].[Login] OFF
SET IDENTITY_INSERT [dbo].[Producto] ON 

INSERT [dbo].[Producto] ([id], [SKU], [nombre], [cantidad], [precio], [descripcion], [imagen]) VALUES (1, NULL, N'telefono', N'24', CAST(200.00 AS Decimal(10, 2)), NULL, NULL)
INSERT [dbo].[Producto] ([id], [SKU], [nombre], [cantidad], [precio], [descripcion], [imagen]) VALUES (2, N'SK20', N'Audifonos Skullcandy', N'25', CAST(50.00 AS Decimal(10, 2)), N'Audifonos inalambricos Skullcandy', 0x4040)
SET IDENTITY_INSERT [dbo].[Producto] OFF
