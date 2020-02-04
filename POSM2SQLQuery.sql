CREATE TABLE [BSJobs].[admin].[POSM2] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Id_Req]          INT            NULL,
    [Link]			  NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [pvgkey3] FOREIGN KEY ([Id_Req]) REFERENCES [BSJobs].[admin].[POSM] ([Id])
);