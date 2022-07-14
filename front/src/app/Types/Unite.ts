export type Unite =
{
    Id: number,
    Influance: number,
    NomImg: string,
    Nom: string,

    IdCouleur: number,
    Couleur: string,

    IdTypeUnite: number,
    NomTypeUnite: string,

    EstMeta: boolean,
    EstVisible: boolean,
    EstTemporaire: boolean,

    EstChoisi?: number,
    Niveau?: string
}