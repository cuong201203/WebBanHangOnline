USE [master]
GO
/****** Object:  Database [WebBanHangOnline]    Script Date: 18/10/2024 5:14:53 pm ******/
CREATE DATABASE [WebBanHangOnline]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebBanHangOnline', FILENAME = N'D:\NeedleServer\Data\WebBanHangOnline.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebBanHangOnline_log', FILENAME = N'D:\NeedleServer\Log\WebBanHangOnline_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [WebBanHangOnline] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebBanHangOnline].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebBanHangOnline] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [WebBanHangOnline] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebBanHangOnline] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebBanHangOnline] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET  ENABLE_BROKER 
GO
ALTER DATABASE [WebBanHangOnline] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebBanHangOnline] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [WebBanHangOnline] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WebBanHangOnline] SET  MULTI_USER 
GO
ALTER DATABASE [WebBanHangOnline] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebBanHangOnline] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebBanHangOnline] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebBanHangOnline] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebBanHangOnline] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WebBanHangOnline] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WebBanHangOnline', N'ON'
GO
ALTER DATABASE [WebBanHangOnline] SET QUERY_STORE = OFF
GO
USE [WebBanHangOnline]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Adv]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Adv](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Image] [nvarchar](500) NULL,
	[Link] [nvarchar](500) NULL,
	[Type] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[SubTitle] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_dbo.Adv] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Cart] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItem]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductName] [nvarchar](max) NULL,
	[Alias] [nvarchar](max) NULL,
	[CategoryName] [nvarchar](max) NULL,
	[ProductImg] [nvarchar](max) NULL,
	[Price] [int] NOT NULL,
	[TotalPrice] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CartItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SeoTitle] [nvarchar](150) NULL,
	[SeoDescription] [nvarchar](250) NULL,
	[SeoKeywords] [nvarchar](150) NULL,
	[Position] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Alias] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[Website] [nvarchar](max) NULL,
	[Message] [nvarchar](4000) NULL,
	[IsRead] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Detail] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
	[CategoryId] [int] NOT NULL,
	[SeoTitle] [nvarchar](max) NULL,
	[SeoDescription] [nvarchar](max) NULL,
	[SeoKeywords] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Alias] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsHome] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.News] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[CustomerName] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[TotalAmount] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[TypePayment] [int] NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[CustomerId] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductName] [nvarchar](max) NULL,
	[Alias] [nvarchar](max) NULL,
	[CategoryName] [nvarchar](max) NULL,
	[ProductImg] [nvarchar](max) NULL,
	[TotalPrice] [int] NOT NULL,
 CONSTRAINT [PK_dbo.OrderDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Detail] [nvarchar](max) NULL,
	[Image] [nvarchar](250) NULL,
	[CategoryId] [int] NOT NULL,
	[SeoTitle] [nvarchar](250) NULL,
	[SeoDescription] [nvarchar](500) NULL,
	[SeoKeywords] [nvarchar](250) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Alias] [nvarchar](150) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Post] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	[ProductCode] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Detail] [nvarchar](max) NULL,
	[Image] [nvarchar](250) NULL,
	[Price] [int] NOT NULL,
	[PriceSale] [int] NULL,
	[Quantity] [int] NOT NULL,
	[IsHome] [bit] NOT NULL,
	[IsSale] [bit] NOT NULL,
	[IsFeature] [bit] NOT NULL,
	[IsHot] [bit] NOT NULL,
	[SeoTitle] [nvarchar](250) NULL,
	[SeoDescription] [nvarchar](500) NULL,
	[SeoKeywords] [nvarchar](250) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Alias] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[OriginalPrice] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Icon] [nvarchar](250) NULL,
	[SeoTitle] [nvarchar](250) NULL,
	[SeoDescription] [nvarchar](500) NULL,
	[SeoKeywords] [nvarchar](250) NULL,
	[Position] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Alias] [nvarchar](150) NULL,
 CONSTRAINT [PK_dbo.ProductCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImage]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Image] [nvarchar](max) NULL,
	[IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ProductImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReviewProducts]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReviewProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[Rate] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Avatar] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ReviewProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscribe]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscribe](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Subscribe] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemSetting]    Script Date: 18/10/2024 5:14:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSetting](
	[SettingKey] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](4000) NULL,
	[SettingDescription] [nvarchar](4000) NULL,
 CONSTRAINT [PK_dbo.SystemSetting] PRIMARY KEY CLUSTERED 
(
	[SettingKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CartId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_CartId] ON [dbo].[CartItem]
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_ProductId] ON [dbo].[CartItem]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CategoryId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_CategoryId] ON [dbo].[News]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_OrderId] ON [dbo].[OrderDetail]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_ProductId] ON [dbo].[OrderDetail]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CategoryId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_CategoryId] ON [dbo].[Post]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductCategoryId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_ProductCategoryId] ON [dbo].[Product]
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_ProductId] ON [dbo].[ProductImage]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductId]    Script Date: 18/10/2024 5:14:55 pm ******/
CREATE NONCLUSTERED INDEX [IX_ProductId] ON [dbo].[ReviewProducts]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Adv] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Adv] ADD  DEFAULT ('') FOR [SubTitle]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CartItem] ADD  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[CartItem] ADD  DEFAULT ((0)) FOR [TotalPrice]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT ((0)) FOR [IsHome]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [TypePayment]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ('') FOR [Email]
GO
ALTER TABLE [dbo].[OrderDetail] ADD  DEFAULT ((0)) FOR [TotalPrice]
GO
ALTER TABLE [dbo].[Post] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [OriginalPrice]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CartItem_dbo.Cart_CartId] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_dbo.CartItem_dbo.Cart_CartId]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CartItem_dbo.Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_dbo.CartItem_dbo.Product_ProductId]
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_dbo.News_dbo.Category_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_dbo.News_dbo.Category_CategoryId]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderDetail_dbo.Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_dbo.OrderDetail_dbo.Order_OrderId]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderDetail_dbo.Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_dbo.OrderDetail_dbo.Product_ProductId]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Post_dbo.Category_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_dbo.Post_dbo.Category_CategoryId]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.ProductCategory_ProductCategoryId] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.ProductCategory_ProductCategoryId]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProductImage_dbo.Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [FK_dbo.ProductImage_dbo.Product_ProductId]
GO
ALTER TABLE [dbo].[ReviewProducts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReviewProducts_dbo.Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReviewProducts] CHECK CONSTRAINT [FK_dbo.ReviewProducts_dbo.Product_ProductId]
GO
USE [master]
GO
ALTER DATABASE [WebBanHangOnline] SET  READ_WRITE 
GO
