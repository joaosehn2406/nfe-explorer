import { FreightMode } from './enums/freight-mode';

export interface Carrier {
  id: string;
  legalName: string;
  cnpj: string | null;
  cpf: string | null;
  stateRegistration: string | null;
  city: string | null;
  uf: string | null;
  freightMode: FreightMode;
}
