import { SessaoValidacaoMensagem } from "../../../Models/SessaoValidacaoMensagem";

export class AlertHelper {

  static createList(
    items: string[],
    className = 'warning-list'
  ): string {
    return `
      <ul class="${className}">
        ${items.map(i => `<li>${i}</li>`).join('')}
      </ul>
    `;
  }

  static createValidationList(
    mensagens: SessaoValidacaoMensagem[],
    className = 'error-list'
  ): string {

    return `
      <ul class="${className}">
        ${mensagens.map(m =>
          `<li>${m.tipo}: ${m.mensagem}.</li>`
        ).join('')}
      </ul>
    `;
  }
}
