export type ParticipantGvG = 
{
    Id: number,
    Date: string,
    ListeCompte: Participant[]
}

type Participant =
{
    Id: number,
    Pseudo: string,
    Influance: number,
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