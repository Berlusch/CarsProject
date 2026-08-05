## npm audit - known exception

**GHSA-qwww-vcr4-c8h2** (react-router-dom, high)
Not applicable — this project does not use React Router's RSC/Framework 
mode, only classic client-side routing (SPA with Vite). The vulnerability 
only affects the unstable RSC APIs.

Decision: staying on the current v7 line until a full migration to 
react-router v8 is planned (requires import changes in the code).

Last reviewed: 2026-08-05