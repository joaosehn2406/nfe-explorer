export interface Recipient {
  legalName: string;
  cnpj: string | null;
  cpf: string | null;
  stateRegistration: string | null;
  city: string;
  zipCode: string;
}
