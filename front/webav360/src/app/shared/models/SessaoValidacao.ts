import { SessaoValidacaoMensagem } from "./SessaoValidacaoMensagem";

export interface SessaoValidacao
{
  podeIniciar: boolean,
  mensagens: SessaoValidacaoMensagem[]
}
