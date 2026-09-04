const URL_API = 'https://localhost:7081/api/v1/Carteira';
let modoEdicao = false; // Declaração da variável que estava faltando

document.addEventListener("DOMContentLoaded", listarCarteiras);

async function listarCarteiras() {
    try {
        const response = await fetch(URL_API);
        if (!response.ok) return;

        const carteiras = await response.json();
        const corpo = document.getElementById('corpoTabela');
        corpo.innerHTML = '';

        carteiras.forEach(c => {
            corpo.innerHTML += `
                <tr>
                    <td>${c.numeroCarteira}</td>
                    <td>${c.nomeCarteira}</td>
                    <td>${c.apetiteCarteira.toFixed(2)}</td>
                    <td>
                        <button class="btn-editar" onclick="prepararEdicao(${c.numeroCarteira}, '${c.nomeCarteira}', ${c.apetiteCarteira})">Editar</button>
                        <button class="btn-excluir" onclick="excluirCarteira(${c.numeroCarteira})">Excluir</button>
                    </td>
                </tr>`;
        });
    } catch (error) {
        console.error("Erro ao listar carteiras:", error);
    }
}

async function salvar() {
    const num = document.getElementById('numCarteira').value;
    const nome = document.getElementById('nomeCarteira').value;
    const apetite = document.getElementById('apetite').value;

    if (!num || !nome || !apetite) {
        alert("Preencha todos os campos antes de prosseguir.");
        return;
    }

    const carteira = {
        numeroCarteira: parseInt(num),
        nomeCarteira: nome,
        apetiteCarteira: parseFloat(apetite)
    };

    const metodo = modoEdicao ? 'PUT' : 'POST';
    const urlFinal = modoEdicao ? `${URL_API}/${num}` : URL_API;

    try {
        const response = await fetch(urlFinal, {
            method: metodo,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(carteira)
        });

        if (response.ok) {
            alert(modoEdicao ? "Carteira atualizada com sucesso!" : "Carteira cadastrada com sucesso!");
            limparCampos();
            await listarCarteiras();
        } else {
            const erroMensagem = await response.text();
            alert(`Erro: ${erroMensagem}`);
        }
    } catch (error) {
        alert("Erro ao conectar com a API.");
    }
}

function prepararEdicao(num, nome, apetite) {
    document.getElementById('numCarteira').value = num;
    document.getElementById('numCarteira').disabled = true;
    document.getElementById('nomeCarteira').value = nome;
    document.getElementById('apetite').value = apetite;
    modoEdicao = true;
}

async function excluirCarteira(num) {
    if (confirm(`Deseja realmente excluir a carteira ${num}?`)) {
        try {
            const response = await fetch(`${URL_API}/${num}`, { method: 'DELETE' });
            if (response.ok) {
                await listarCarteiras();
            }
        } catch (error) {
            alert("Erro ao excluir carteira.");
        }
    }
}

function limparCampos() {
    document.getElementById('numCarteira').value = '';
    document.getElementById('numCarteira').disabled = false;
    document.getElementById('nomeCarteira').value = '';
    document.getElementById('apetite').value = '';
    modoEdicao = false;
}