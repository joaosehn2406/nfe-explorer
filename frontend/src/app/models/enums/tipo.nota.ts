export enum TipoNota {
  Entrada = 0,
  Saida = 1,
}

export function tipoNotaResolver(id: any) {
  if (id === 1) {
    return "Saída"
  }

  return "Entrada"
}
