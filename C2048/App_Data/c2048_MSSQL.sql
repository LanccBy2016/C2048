CREATE TABLE [dbo].[Users] (
    [UserID]     NVARCHAR (50) NOT NULL,
    [Pwd]        NVARCHAR (50) NOT NULL,
    [CreateTime] DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);


CREATE TABLE [dbo].[UserScoreLog] (
    [UserID]     NVARCHAR (50) NOT NULL,
    [Score]      INT           NOT NULL,
    [CreateTime] DATETIME      DEFAULT (getdate()) NULL
);
