import { MesUnite } from "../import/MesUnite"

export type ConfigCompteExport = 
{
    IdCompte: number,
    IdClasseHeros: number,
    Pseudo: string,
    Influance: number,
    IdDiscord: string,
    ListeUniteNiv: MesUnite[]
}