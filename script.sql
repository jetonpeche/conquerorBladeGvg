DROP TABLE UniteCompte;
DROP TABLE GvgUniteCompte;
DROP TABLE GvgCompte;
DROP TABLE Gvg;
DROP TABLE Compte;
DROP TABLE Unite;
DROP TABLE CouleurUnite;
DROP TABLE TypeUnite;
DROP TABLE ClasseHeros;

-- compte delete cascade

CREATE TABLE ClasseHeros
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    nom varchar(100) NOT NULL,
    iconClasse varchar(100) NOT NULL,
    nomImg varchar(150) NOT NULL
);

CREATE TABLE Gvg
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    dateProgrammer date NOT NULL
);

CREATE TABLE TypeUnite
( 
    id int PRIMARY KEY NOT NULL,
    nom varchar(100) NOT NULL
);

CREATE TABLE CouleurUnite
( 
    id int PRIMARY KEY NOT NULL,
    nom varchar(100) NOT NULL
);

CREATE TABLE Unite
( 
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    idCouleur int NOT NULL,
    idTypeUnite int NOT NULL,
    nom varchar(100) NOT NULL,
    nomImg varchar(100) NOT NULL,
    influance int NOT NULL,

    FOREIGN KEY (idCouleur) REFERENCES CouleurUnite(id),
    FOREIGN KEY (idTypeUnite) REFERENCES TypeUnite(id)
);

CREATE TABLE Compte
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    idClasseHeros int NOT NULL,
    idDiscord varchar(100) NULL,
    nomDiscord varchar NULL,
    pseudo varchar(150) NOT NULL,
    influance int NOT NULL,
    estPremiereConnexion int DEFAULT 1,

    FOREIGN KEY (idClasseHeros) REFERENCES ClasseHeros(id)
);

CREATE TABLE GvgCompte
(
    idGvg int NOT NULL,
    idCompte int NOT NULL,

    PRIMARY KEY (idGvg, idCompte),

    FOREIGN KEY (idGvg) REFERENCES Gvg(id),
    FOREIGN KEY (idCompte) REFERENCES Compte(id) ON DELETE CASCADE
);

CREATE TABLE GvgUniteCompte
(
    idGvg int NOT NULL,
    idCompte int NOT NULL,
    idUnite int NULL,

    PRIMARY KEY (idGvg, idCompte),

    FOREIGN KEY (idGvg) REFERENCES Gvg(id),
    FOREIGN KEY (idCompte) REFERENCES Compte(id) ON DELETE CASCADE,
    FOREIGN KEY (idUnite) REFERENCES Unite(id)
);

CREATE TABLE UniteCompte
( 
    idCompte int NOT NULL,
    idUnite int NOT NULL,
    niveauMaitrise varchar(30) NOT NULL,

    PRIMARY KEY(idCompte, idUnite),

    FOREIGN KEY (idCompte) REFERENCES Compte(id) ON DELETE CASCADE,
    FOREIGN KEY (idUnite) REFERENCES Unite(id)
);

INSERT INTO TypeUnite (id, nom) VALUES (1, 'Distance'), (2, 'Cavalerie'), (3, 'Infanterie');
INSERT INTO CouleurUnite (id, nom) VALUES (1, 'Bleu'), (2, 'Violet'), (3, 'Jaune');

SET IDENTITY_INSERT Unite ON;
INSERT INTO Unite(id, idCouleur, idTypeUnite, nom, nomImg, influance) VALUES
(1, 2, 3, 'Lanceur de hache', 'axe_raiders_thumb.png', 240), 
(2, 2, 3, 'Azaps', 'azap_thumb.png', 240),
(3, 1, 3, 'Bard', 'bagpipers_thumb.png', 120),
(4, 3, 3, 'Siphonaros', 'barc-narf-gua-thumb.png', 330),
(5, 2, 3, 'Berserker', 'berserker_thumb.png', 245),
(6, 3, 1, 'Falconetti', 'falco_gun.png', 330),
(7, 3, 2, 'Pyro-lancier', 'fire_lan_pre.png', 305),
(8, 2, 3, 'Piquier de fortebraccio', 'forte_pike.png',235),
(9, 2, 3, 'Vigiles grisonnants', 'greyhair_garrison_thumb.png', 240),
(10, 2, 1, 'Archers impériaux', 'imp_arc_pre.png', 255),
(11, 2, 3, 'Javeliniers impériaux', 'imp_jav_pre.png', 240),
(12, 2, 3, 'Piquiers impériaux', 'imp_pk_pre.png', 240),
(13, 2, 3, 'Lanciers impériaux', 'imp_spea_guar_pre.png', 245),
(14, 3, 3, 'Fauchefers', 'ir_rea_pre.png', 315),
(15, 3, 2, 'Kheshigs', 'kheshigs.png', 310),
(16, 3, 3, 'Modao', 'modao_battalion_thumb.png', 315),
(17, 3, 2, 'Templiers', 'mona_kni.png', 305),
(18, 1, 1, 'Archer de mamkahn', 'nam_arch.png', 180),
(19, 2, 3, 'Gardes du palais', 'pala_guar_pre.png', 235),
(20, 3, 1, 'Arbalétriers pavoiseurs', 'pavi_cros.png', 325),
(21, 1, 3, 'Gardes préfectoraux', 'pref_guar_pre.png', 180),
(22, 3, 1, 'Grenadiers de Senji', 'shenji_grenadiers_thumb.png', 315),
(23, 3, 3, 'Silahdars', 'sihl_thumb.png', 320),
(24, 2, 2, 'Sipahis', 'sipahi_thumb.png', 245),
(25, 2, 3, 'Paladins symmachéens', 'symm_pal_thumb.png', 255),
(26, 3, 1, 'Arquebusiers tercios', 'tercio.png', 310),
(27, 3, 2, 'Hussards ailés', 'win_hus_pre.png', 315);
SET IDENTITY_INSERT Unite OFF;

SET IDENTITY_INSERT ClasseHeros ON;
INSERT INTO ClasseHeros (id, nom, nomImg, iconClasse) VALUES 
(1, 'Guandao', 'guandao.jpg', 'guandao.webp'), (2, 'Hache d arme', 'hacheArme.jpg', 'hacheArme.webp'), 
(3, 'Lance', 'lance.jpg', 'lance.webp'), (4, 'Arc court', 'arcCourt.jpg', 'arcCourt.webp'), 
(5, 'Lames jumelles', 'lameJumelle.jpg', 'assassin.webp'),
(6, 'Masse de guerre', 'masse.jpg', 'marteau.webp'), (7, 'Nodachi', 'nodachi.jpg', 'nodachi.webp'), 
(8, 'Epee longue', 'epeeLongue.jpg', 'epeeLongue.webp'), 
(9, 'Mousquet', 'mousquet.jpg', 'musket.webp'), (10, 'Arc long', 'arcLong.jpg', 'arcLong.webp'), (11, 'Epee courte', 'epeeCourte.jpg', 'epeeCourt.webp'), 
(12, 'Pique', 'lance.jpg', 'pique.webp');
SET IDENTITY_INSERT ClasseHeros OFF;

SET IDENTITY_INSERT Compte ON;
INSERT INTO Compte (id, idClasseHeros, pseudo, influance) VALUES (1, 4, 'Jetonpeche', 730), (2, 5, 'test', 700);
SET IDENTITY_INSERT Compte OFF;