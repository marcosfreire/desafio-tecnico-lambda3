export class ProfessorViewModel {
    constructor(
        public id?: number,        
        public nome?: string,
        public dataNascimento?: Date,
        public disciplinas?:Course[],
        public sobrenome?:string,
    ) {}
}

export interface Course {
    id: string;
    name: string;
}