export class Oncall {
    constructor(
    public id: string,
    public year: number,
    public week: number,
    public team: string,
    public primary: string,
    public backup: string
    ){ }
}