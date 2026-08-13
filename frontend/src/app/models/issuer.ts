export interface Issuer {
  legalName: string;
  tradeName: string | null;
  cnpj: string;
  stateRegistration: string | null;
  city: string;
  uf: string;
  zipCode: string;
}
