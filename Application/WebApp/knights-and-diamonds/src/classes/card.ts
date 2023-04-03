export interface Card {
    id?: number;
    cardName: string;
    cardType: string;
    numberOfCardsAffected: number;
    pointsAddedLost: number;
    cardEffect: string;
    cardLevel: number;
    attackPoints: number;
    defencePoints: number;
    imgPath: any;
    description: any;
}