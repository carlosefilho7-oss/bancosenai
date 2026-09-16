const URL_API = 'https://localhost:7081/api/v1/Documentacao'
async function enviardocumento() {
    const codigocliente = document.getElementById("codigocliente").value;
    const inputarquivo = document.getElementById("arquivo");
    const arquivo = inputArquivo.files[0];

    if (!codigocliente || !arquivo) {
        alert("informe o codigo do cliente e selecione um arquivo");
        return;
    }

    const dadosarquivo = new FormData();
    dadosarquivo.append("arquivo", arquivo);

    const response = await fetch(`${URL_API}/upload/${codigocliente}`, {
        method: "POST",
        body: dadosarquivo
    });

    if (response.ok) {
        alert("documento enviado com sucesso!");
        document.getElementById("codigocliente").value = "";
        document.getElementById("arquivo").value = "";
    }
}