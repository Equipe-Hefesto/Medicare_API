/* MedicareBD: */

CREATE TABLE Utilizadores (
    idUtilizador INT NOT NULL,
    nmUtilizador VARCHAR(40) NOT NULL,
    sbUtilizador VARCHAR(40) NOT NULL,
    cpfUtilizador CHAR(11) NOT NULL,
    emUtilizador VARCHAR(255) NOT NULL,
    telUtilizador CHAR(11) NOT NULL,
    senhaHash VARCHAR(128) NOT NULL,
    senhaSalt VARCHAR(64) NOT NULL,
    userUtilizador VARCHAR(30) NOT NULL
);
GO

ALTER TABLE Utilizadores ADD CONSTRAINT PK_Utilizadores PRIMARY KEY (idUtilizador);
GO

CREATE TABLE TiposUtilizadores (
    idTipoUtilizador INT NOT NULL,
    dsTipoUtilizador VARCHAR(14) NOT NULL
);
GO

ALTER TABLE TiposUtilizadores ADD CONSTRAINT PK_TiposUtilizadores PRIMARY KEY (idTipoUtilizador);
GO

CREATE TABLE TiposParentesco (
    idTipoParentesco INT NOT NULL,
    dsParentesco VARCHAR(20) NOT NULL
);
GO

ALTER TABLE TiposParentesco ADD CONSTRAINT PK_TiposParentesco PRIMARY KEY (idTipoParentesco);
GO

CREATE TABLE Responsaveis (
    idResponsavel INT NOT NULL,
    idPaciente INT NOT NULL,
    idTipoParentesco INT NOT NULL,
    dcResponsabilidade DATETIME NOT NULL,
    duResponsabilidade DATETIME NOT NULL,
    stResponsabilidade CHAR(1) NOT NULL
);
GO

ALTER TABLE Responsaveis ADD CONSTRAINT PK_Responsaveis PRIMARY KEY (idPaciente, idResponsavel);
GO

CREATE TABLE Cuidadores (
    idCuidador INT NOT NULL,
    idPaciente INT NOT NULL,
    dtInicio DATETIME NOT NULL,
    dtFim DATETIME NOT NULL,
    dcCuidado DATETIME NOT NULL,
    duCuidado DATETIME NOT NULL,
    stCuidado CHAR(1) NOT NULL
);
GO

ALTER TABLE Cuidadores ADD CONSTRAINT PK_Cuidadores PRIMARY KEY (idPaciente, idCuidador);
GO

CREATE TABLE Parceiros (
    idParceiro INT NOT NULL,
    nmParceiro VARCHAR(50) NOT NULL,
    apParceiro VARCHAR(25) NOT NULL,
    cnpjParceiro CHAR(18) NOT NULL,
    stParceiro CHAR(1) NOT NULL
);
GO

ALTER TABLE Parceiros ADD CONSTRAINT PK_Parceiros PRIMARY KEY (idParceiro);
GO

CREATE TABLE UtilizadoresTiposUtilizadores (
    idUtilizador INT NOT NULL,
    idTipoUtilizador INT NOT NULL
);
GO

ALTER TABLE UtilizadoresTiposUtilizadores ADD CONSTRAINT PK_UtilizadorTiposUtilizadores PRIMARY KEY (idUtilizador, idTipoUtilizador);
GO

CREATE TABLE ParceirosUtilizadores (
    idParceiro INT NOT NULL,
    idUtilizador INT NOT NULL
);
GO

ALTER TABLE ParceirosUtilizadores ADD CONSTRAINT PK_ParceirosUtilizadores PRIMARY KEY (idParceiro, idUtilizador);
GO

CREATE TABLE TiposGrandeza (
    idTipoGrandeza INT NOT NULL,
    dsGrandeza VARCHAR(20) NOT NULL
);
GO

ALTER TABLE TiposGrandeza ADD CONSTRAINT PK_TiposGrandeza PRIMARY KEY (idTipoGrandeza);
GO

CREATE TABLE TiposAgendamento (
    idTipoAgendamento INT NOT NULL,
    dsTipoAgendamento VARCHAR(40) NOT NULL
);
GO

ALTER TABLE TiposAgendamento ADD CONSTRAINT PK_TiposAgendamento PRIMARY KEY (idTipoAgendamento);
GO

CREATE TABLE TiposFarmaceutico (
    idTipoFarmaceutico INT NOT NULL,
    dsTipoFarmaceutico VARCHAR(40) NOT NULL
);
GO

ALTER TABLE TiposFarmaceutico ADD CONSTRAINT PK_TiposFarmaceutico PRIMARY KEY (idTipoFarmaceutico);
GO

CREATE TABLE Remedios (
    idRemedio INT NOT NULL,
    nmRemedio VARCHAR(40) NOT NULL,
    dcRemedio DATETIME NOT NULL,
    duRemedio DATETIME NOT NULL
);
GO

ALTER TABLE Remedios ADD CONSTRAINT PK_Remedios PRIMARY KEY (idRemedio);
GO

CREATE TABLE Posologias (
    idPosologia INT NOT NULL,
    idRemedio INT NOT NULL,
    idUtilizador INT NOT NULL,
    qtdePosologia INT NOT NULL,
    idTipoFarmaceutico INT NOT NULL,
    qtdeDose INT NOT NULL,
    idTipoGrandeza INT NOT NULL,
    diPosologia DATETIME NOT NULL,
    dfPosologia DATETIME NOT NULL,
    idTipoAgendamento INT NOT NULL,
    intervalo INT NOT NULL,
    diasSemana VARCHAR(16) NOT NULL,
    diasUso INT NOT NULL,
    diasPausa INT NOT NULL
);
GO

ALTER TABLE Posologias ADD CONSTRAINT PK_Posologias PRIMARY KEY (idPosologia);
GO

CREATE TABLE Horarios (
    idPosologia INT NOT NULL,
    horario TIME NOT NULL
);
GO

ALTER TABLE Horarios ADD CONSTRAINT PK_Horarios PRIMARY KEY (idPosologia, horario);
GO

CREATE TABLE Alarmes (
    idAlarme INT NOT NULL,
    idPosologia INT NOT NULL,
    dtHoraAlarme DATETIME NOT NULL,
    dsAlarme VARCHAR(40) NOT NULL,  
    stAlarme CHAR(1) NOT NULL
);
GO

ALTER TABLE Alarmes ADD CONSTRAINT PK_Alarmes PRIMARY KEY (idAlarme);
GO

CREATE TABLE FormasPagamento (
    idFormaPagamento INT NOT NULL,
    dsFormaPagamento VARCHAR(45) NOT NULL,
    qtdeParcelas INT NOT NULL,
    qtdeMinParcelas INT NOT NULL,
    qtdeMaxParcelas INT NOT NULL
);
GO

ALTER TABLE FormasPagamento ADD CONSTRAINT PK_FormasPagamento PRIMARY KEY (idFormaPagamento);
GO

CREATE TABLE Promocoes (
    idPromocao INT NOT NULL,
    idFormaPagamento INT NOT NULL,
    idUtilizador INT NOT NULL,
    dsPromocao VARCHAR(50) NOT NULL,
    idRemedio INT NOT NULL,
    dtInicio DATETIME NOT NULL,
    dtFim DATETIME NOT NULL,
    vlrPromocao DECIMAL
);
GO

ALTER TABLE Promocoes ADD CONSTRAINT PK_Promocoes PRIMARY KEY (idPromocao);
GO

ALTER TABLE Responsaveis ADD CONSTRAINT FK_Responsaveis_2
    FOREIGN KEY (idResponsavel) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE Responsaveis ADD CONSTRAINT FK_Responsaveis_3
    FOREIGN KEY (idPaciente) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE Responsaveis ADD CONSTRAINT FK_Responsaveis_4
    FOREIGN KEY (idTipoParentesco) REFERENCES TiposParentesco (idTipoParentesco);
GO

ALTER TABLE Cuidadores ADD CONSTRAINT FK_Cuidadores_2
    FOREIGN KEY (idCuidador) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE Cuidadores ADD CONSTRAINT FK_Cuidadores_3
    FOREIGN KEY (idPaciente) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE UtilizadoresTiposUtilizadores ADD CONSTRAINT FK_UtilizadoresTiposUtilizadores_2
    FOREIGN KEY (idUtilizador) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE UtilizadoresTiposUtilizadores ADD CONSTRAINT FK_UtilizadoresTiposUtilizadores_3
    FOREIGN KEY (idTipoUtilizador) REFERENCES TiposUtilizadores (idTipoUtilizador);
GO

ALTER TABLE ParceirosUtilizadores ADD CONSTRAINT FK_ParceirosUtilizadores_2
    FOREIGN KEY (idParceiro) REFERENCES Parceiros (idParceiro);
GO

ALTER TABLE ParceirosUtilizadores ADD CONSTRAINT FK_ParceirosUtilizadores_3
    FOREIGN KEY (idUtilizador) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE Posologias ADD CONSTRAINT FK_Posologias_2
    FOREIGN KEY (idRemedio) REFERENCES Remedios (idRemedio);
GO

ALTER TABLE Posologias ADD CONSTRAINT FK_Posologias_3
    FOREIGN KEY (idUtilizador) REFERENCES Utilizadores (idUtilizador);
GO

ALTER TABLE Posologias ADD CONSTRAINT FK_Posologias_4
    FOREIGN KEY (idTipoFarmaceutico) REFERENCES TiposFarmaceutico (idTipoFarmaceutico);
GO

ALTER TABLE Posologias ADD CONSTRAINT FK_Posologias_5
    FOREIGN KEY (idTipoGrandeza) REFERENCES TiposGrandeza (idTipoGrandeza);
GO

ALTER TABLE Posologias ADD CONSTRAINT FK_Posologias_6
    FOREIGN KEY (idTipoAgendamento) REFERENCES TiposAgendamento (idTipoAgendamento);
GO

ALTER TABLE Horarios ADD CONSTRAINT FK_Horarios_2
    FOREIGN KEY (idPosologia) REFERENCES Posologias (idPosologia);
GO

ALTER TABLE Alarmes ADD CONSTRAINT FK_Alarmes_2
    FOREIGN KEY (idPosologia) REFERENCES Posologias (idPosologia);
GO

ALTER TABLE Promocoes ADD CONSTRAINT FK_Promocoes_2
    FOREIGN KEY (idFormaPagamento) REFERENCES FormasPagamento (idFormaPagamento);
GO

ALTER TABLE Promocoes ADD CONSTRAINT FK_Promocoes_3
    FOREIGN KEY (idUtilizador) REFERENCES Utilizadores (idUtilizador);
GO
