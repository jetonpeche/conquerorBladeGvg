export type GvgParametrer =
{
    Nom: string,
    ListeJoueur: Joueur[]
}

type Joueur = 
{
    Pseudo: string,
    ListeUnite: UniteJoueur[]
}

type UniteJoueur =
{
    Nom: string,
    Influance: number
}