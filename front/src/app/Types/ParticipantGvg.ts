export type ParticipantGvG = 
{
    Id: number,
    Date: string,
    ListeCompte: Participant[]
}

export type Participant =
{
    Id: number,
    Pseudo: string,
    Influance: number,
    IdGroupe: number,
    ListeUnite: UniteParticipant[]
}

export type UniteParticipant =
{
    Id: number,
    Nom: string,
    Influance: number,
    NiveauMaitrise: string,
    EstDejaChoisi: boolean
}