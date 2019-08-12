import { DisciplinaViewModel } from './DisciplinaViewModel';

export class ProfessorViewModel {
    constructor(
        public id?: number,        
        public nome?: string,
        public dataNascimento?: Object,
        public disciplinas?:DisciplinaViewModel[],
        public sobrenome?:string,
    ) {}
}