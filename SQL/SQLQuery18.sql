USE Autoveille
GO

/****** Object:  Table atv.TbRelanceAutoveille    Script Date: 03-01-2021 00:21:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE atv.TbRelanceAutoveille(
	Id int IDENTITY(1,1) NOT NULL,
	NoClient int NOT NULL,
	NoSerie varchar(30) NULL,
	Nom nvarchar(30) NULL,
	Prenom nvarchar(30) NULL,
	Compagnie nvarchar(100) NULL,
	Langue nvarchar(1) NULL,
	TelephoneResidence nvarchar(14) NULL,
	TelephoneTravail nvarchar(14) NULL,
	Cellulaire nvarchar(14) NULL,
	ExtTravail nvarchar(100) NULL,
	Marque nvarchar(50) NULL,
	Modele nvarchar(50) NULL,
	Annee int NULL,
	Email nvarchar(50) NULL,
	NomSignataire nvarchar(50) NULL,
	DateAchat date NULL,
	NbreMois int NULL,
	FinDuTerme date NULL,
	AchatLocation nvarchar(1) NULL,
	EtatVehicule nvarchar(1) NULL,
	IdEvenement int NULL,
	Resultat int NULL,
	DateResultat int NULL,
	Type nvarchar(50) NULL,
	Provenance nvarchar(50) NULL,
	IdRelanceCASuly int NULL,
	RaisonDesactivation nvarchar(50) NULL,
	NoteSulyService nvarchar(max) NULL,
	NoteSulyVente nvarchar(max) NULL,
	NoteAppel nvarchar(max) NULL,
	ValeurVehiculeCBB money NULL,
	DateCalculeValeurVehiculeCBB datetime NULL,
	ProchaineRelance date NULL,
	DateRDVService date NULL,
	HeureRDVService time(7) NULL,
	DateRDV date NULL,
	HleureRDV time(7) NULL,
	DateRDVAnt date NULL,
	HeureRDVAnt time(7) NULL,
	RelanceCASuly nchar(10) NULL,
	DateEquite datetime NULL,
	Date60Terme datetime NULL,
 CONSTRAINT PK_TbRelanceAutoveille PRIMARY KEY CLUSTERED 
(
	Id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON PRIMARY
) ON PRIMARY TEXTIMAGE_ON PRIMARY

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Autoveille financement équité, Autoveille loaction équité, Autoveille location fin du terme, Vente, Walk-out, Leads, RDVSErvice, ' , @level0type=N'SCHEMA',@level0name=N'atv', @level1type=N'TABLE',@level1name=N'TbRelanceAutoveille', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Autoveille, tacktik, manuelle, no-show RDV, walk-out ' , @level0type=N'SCHEMA',@level0name=N'atv', @level1type=N'TABLE',@level1name=N'TbRelanceAutoveille', @level2type=N'COLUMN',@level2name=N'Provenance'
GO


