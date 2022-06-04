DROP TABLE UniteCompte;
DROP TABLE GvgUniteCompte;
DROP TABLE GvgCompte;
DROP TABLE Gvg;
DROP TABLE Compte;
DROP TABLE Unite;
DROP TABLE CouleurUnite;
DROP TABLE TypeUnite;
DROP TABLE ClasseHeros;
DROP TABLE Groupe;

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

CREATE TABLE Groupe
(
    id int PRIMARY KEY NOT NULL,
    nom varchar(50) NOT NULL
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
    pseudo varchar(150) NOT NULL,
    influance int NOT NULL,
    estPremiereConnexion int DEFAULT 1,
    estAdmin int NOT NULL DEFAULT 0,

    FOREIGN KEY (idClasseHeros) REFERENCES ClasseHeros(id)
);

CREATE TABLE GvgCompte
(
    idGvg int NOT NULL,
    idCompte int NOT NULL,
    idGroupe int,

    PRIMARY KEY (idGvg, idCompte),

    FOREIGN KEY (idGvg) REFERENCES Gvg(id),
    FOREIGN KEY (idGroupe) REFERENCES Groupe(id),
    FOREIGN KEY (idCompte) REFERENCES Compte(id) ON DELETE CASCADE
);

CREATE TABLE GvgUniteCompte
(
    idGvg int NOT NULL,
    idCompte int NOT NULL,
    idUnite int NOT NULL,

    PRIMARY KEY (idGvg, idCompte, idUnite),

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

INSERT INTO Groupe (id, nom) VALUES (1, 'Groupe 1'), (2, 'Groupe 2'), (3, 'Groupe 3'), (4, 'Groupe 4'),
                                    (5, 'Groupe 5'), (6, 'Groupe 6'), (7, 'Groupe 7'), (8, 'Groupe 8');
INSERT INTO TypeUnite (id, nom) VALUES (1, 'Distance'), (2, 'Cavalerie'), (3, 'Infanterie');
INSERT INTO CouleurUnite (id, nom) VALUES (1, 'Blanc'), (2, 'Vert'), (3, 'Bleu'), (4, 'Violet'), (5, 'Jaune');

SET IDENTITY_INSERT Unite ON;
INSERT INTO Unite(id, idCouleur, idTypeUnite, nom, nomImg, influance) VALUES
(1, 4, 3, 'Lanceur de hache', 'axe_raiders_thumb.png', 240), 
(2, 4, 3, 'Azaps', 'azap_thumb.png', 240),
(3, 3, 3, 'Bard', 'bagpipers_thumb.png', 120),
(4, 5, 3, 'Siphonaros', 'barc-narf-gua-thumb.png', 330),
(5, 4, 3, 'Berserker', 'berserker_thumb.png', 245),
(6, 5, 1, 'Falconetti', 'falco_gun.png', 330),
(7, 5, 2, 'Pyro-lancier', 'fire_lan_pre.png', 305),
(8, 4, 3, 'Piquier de fortebraccio', 'forte_pike.png',235),
(9, 4, 3, 'Vigiles grisonnants', 'greyhair_garrison_thumb.png', 240),
(10, 4, 1, 'Archers impériaux', 'imp_arc_pre.png', 255),
(11, 4, 3, 'Javeliniers impériaux', 'imp_jav_pre.png', 240),
(12, 4, 3, 'Piquiers impériaux', 'imp_pk_pre.png', 240),
(13, 4, 3, 'Lanciers impériaux', 'imp_spea_guar_pre.png', 245),
(14, 5, 3, 'Fauchefers', 'ir_rea_pre.png', 315),
(15, 5, 2, 'Kheshigs', 'kheshigs.png', 310),
(16, 5, 3, 'Modao', 'modao_battalion_thumb.png', 315),
(17, 5, 2, 'Templiers', 'mona_kni.png', 305),
(18, 3, 1, 'Archer de mamkahn', 'nam_arch.png', 180),
(19, 4, 3, 'Gardes du palais', 'pala_guar_pre.png', 235),
(20, 5, 1, 'Arbalétriers pavoiseurs', 'pavi_cros.png', 325),
(21, 3, 3, 'Gardes préfectoraux', 'pref_guar_pre.png', 180),
(22, 5, 1, 'Grenadiers de Senji', 'shenji_grenadiers_thumb.png', 315),
(23, 5, 3, 'Silahdars', 'sihl_thumb.png', 320),
(24, 4, 2, 'Sipahis', 'sipahi_thumb.png', 245),
(25, 4, 3, 'Paladins symmachéens', 'symm_pal_thumb.png', 255),
(26, 5, 1, 'Arquebusiers tercios', 'tercio.png', 310),
(27, 5, 2, 'Hussards ailés', 'win_hus_pre.png', 315),
(28, 2, 3, 'Epéiste tête de fer', 'iron_swor_pre.png', 115),
(29, 1, 3, 'Martella', 'martella.png', 30),
(30, 2, 3, 'Javelinier de milice', 'jav_mil_pre.png', 110),
(31, 2, 3, 'Javelinier de domaine', 'dem_jav_pre.png', 110),
(32, 3, 3, 'Piquiers préfectoraux', 'pref_pike_pre.png', 185),
(33, 4, 3, 'Prévôt hallebardiers', 'halb_serg_pre.png', 230),
(34, 3, 3, 'Hallebardiers', 'halb_pre.png', 175),
(35, 2, 3, 'Piquier de milice', 'pk_mil_pre.png', 110),
(36, 2, 1, 'Archers tête de fer', 'iro_arc_pre.png', 135),
(37, 4, 1, 'Archers gallos', 'vas_long_pre.png', 245),
(38, 3, 2, 'Pionners du selem', 'selem_cav.png', 170),
(39, 3, 2, 'khorchins', 'khorchins.png', 170),
(40, 3, 3, 'Condottières', 'condo_gua.png', 170),
(41, 3, 1, 'Janissaires', 'janissary_thumb.png', 190),
(42, 3, 3, 'Flis de Fenrir', 'sons_of_fenrir_thumb.png', 180),
(43, 3, 3, 'Moines guerrier', 'cudgel_monks_thumb.png', 190),
(44, 3, 1, 'Pyro archers', 'ind_arc_pre.png', 180);
SET IDENTITY_INSERT Unite OFF;

SET IDENTITY_INSERT ClasseHeros ON;
INSERT INTO ClasseHeros (id, nom, nomImg, iconClasse) VALUES 
(1, 'Guandao', 'guandao.jpg', 'guandao.png'), (2, 'Hache d arme', 'hacheArme.jpg', 'hacheArme.png'), 
(3, 'Lance', 'lance.jpg', 'lance.png'), (4, 'Arc court', 'arcCourt.jpg', 'arcCourt.png'), 
(5, 'Lames jumelles', 'lameJumelle.jpg', 'assassin.png'),
(6, 'Masse de guerre', 'masse.jpg', 'marteau.png'), (7, 'Nodachi', 'nodachi.jpg', 'nodachi.png'), 
(8, 'Epee longue', 'epeeLongue.jpg', 'epeeLongue.png'), 
(9, 'Mousquet', 'mousquet.jpg', 'musket.png'), (10, 'Arc long', 'arcLong.jpg', 'arcLong.png'), (11, 'Epee courte', 'epeeCourte.jpg', 'epeeCourt.png'), 
(12, 'Pique', 'lance.jpg', 'pique.png');
SET IDENTITY_INSERT ClasseHeros OFF;

SET IDENTITY_INSERT Compte ON;
INSERT INTO Compte (id, idClasseHeros, idDiscord, pseudo, influance, estAdmin) VALUES 
(1, 4, '341945414318161920', 'Jetonpeche', 730, 1),
(2, 9, '267788625364647937', 'Jalief', 700, 1)
SET IDENTITY_INSERT Compte OFF;