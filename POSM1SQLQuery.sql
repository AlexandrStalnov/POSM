CREATE TABLE [BSJobs].[admin].[POSM1] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Id_Req]          INT            NULL,
    [Product]         NTEXT          NULL,
    [TotalLoad]       FLOAT (53)     NULL,
    [Pallet]          NVARCHAR (255) NULL,
    [Lamination]      NVARCHAR (255) NULL,
    [Stamp]           NVARCHAR (255) NULL,
    [VarnishForm]     NVARCHAR (255) NULL,
    [QuantityPrint]   VARCHAR (255)  NULL,
    [QuantityNoPrint] VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [pvgkey2] FOREIGN KEY ([Id_Req]) REFERENCES [BSJobs].[admin].[POSM] ([Id])
);