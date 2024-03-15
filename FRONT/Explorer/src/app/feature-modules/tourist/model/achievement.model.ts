export interface Achievement {
    id?: number,
    name: string,
    description: string,
    type: AchievementType,
}
export enum AchievementType {
    HIDDEN_ENCOUNTER = "HIDDEN_ENCOUNTER",
    FIGTH_1 = "FIGTH_1",
    FIGTH_2 = "FIGTH_2",
    SOCIAL = "SOCIAL"
    
}