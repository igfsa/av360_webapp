import { SessaoValidacaoMensagem } from "../../../Models/SessaoValidacaoMensagem";

export class AlertHelper {

  static createList(
    items: string[],
    className = 'warning-list'
  ): string {
    return `
      <div class="${className}">
        ${items.map(i => `<br>${i}`).join('')}
      </div>
    `;
  }

  static createValidationList(
    mensagens: SessaoValidacaoMensagem[],
    className = 'error-list'
  ): string {

    return `
      <div class="${className}">
        ${mensagens.map(m =>
          `<br>${m.tipo}: ${m.mensagem}`
        ).join('')}
      </div>
    `;
  }
}
