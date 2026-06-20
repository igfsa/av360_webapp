// api/index.js
export default async (req, res) => {
  // Importa dinamicamente o handler de requisições gerado pelo Angular SSR
  const { reqHandler } = await import('../dist/webav360/server/server.mjs');
  return reqHandler(req, res);
};
