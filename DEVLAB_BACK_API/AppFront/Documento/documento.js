const URL_API = 'https://localhost:7081/api/v1/Documento'
async function enviardocumento() {
    const codigocliente = document.getElementById("codigocliente").value;
    const inputArquivo = document.getElementById("arquivo");
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
    } else {
        const erro = await response.json();
        alert("Erro!: "+(erro.message || "falha ao enviar a messagem"))
    }
}
async function buscarDocumento() {
    const codCliente = document.getElementById('buscarCliente').value;
    const conteinerResultado = document.getElementById('resultadoBusca');

    if (!codCliente) {
        alert('Por favor, informe o código do cliente.');
        return;
    }
    try {
        const resposta = await fetch(`${URL_API}/listar/${codCliente}`);
        if (!resposta.ok) throw new Error('Erro ao buscar dados.');

        const documentos = await resposta.json();
        conteinerResultado.innerHTML = '';
        if (documentos.length === 0) {
            conteinerResultado.innerHTML = '<p>Nenhum documento encontrado.</p>';
            return;
        }
        let tabelaHTML = `
            <table border="1" style="width: 100%; text-align: left; border-collapse: collapse; margin-top: 15px; background-color:White;th.background-color:Blue;">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nome do Arquivo</th>
                        <th>Extensao</th>
                        <th>Acoes</th>
                    </tr>
                </thead>
                <tbody>
        `;//odeio montar tabela
        documentos.forEach(doc => {
            const id = doc.id || doc.id_documento || '-';
            const nome = doc.nome || doc.nome_arquivo || 'Sem nome';
            const extensao = doc.extensao;
            const url = doc.url || doc.caminho || '#';//foi oqeu recomendou para demonstrar os tipos na tabela

            tabelaHTML += `
                <tr>
                    <td>${id}</td>
                    <td>${nome}</td>
                    <td>${extensao}</td>
                    <td>
                        <a href="${url}" download target="_blank">Baixar</a> | 
                        <button onclick="deletarDocumento(${id})" style="color: red; cursor: pointer;background-color:black;">Deletar</button>
                    </td>
                </tr>
            `;
        });

        tabelaHTML += `
                </tbody>
            </table>
        `;

        conteinerResultado.innerHTML = tabelaHTML;

    } catch (erro) {
        console.error('Erro:', erro);
        conteinerResultado.innerHTML = '<p style="color: red;">Erro ao carregar a tabela.</p>';
    }
}
async function deletarDocumento(id) {
    if (confirm(`você tem certeza que deseja apagar o documento ID ${id}?`)) {
        try {
            const resposta = await fetch(`${URL_API}/excluir/${id}`, {
                method: 'DELETE'
            });

            if (resposta.ok) {
                alert('documento eliminado com sucesso!');
                buscarDocumento(); 
            } else {
                alert('Erro ao tentar apagar o documento.');
            }
        } catch (erro) {
            console.error('Erro ao deletar:', erro);
        }
    }
}