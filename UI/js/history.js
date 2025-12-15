const API_BASE = "https://localhost:44325";
const token = localStorage.getItem("token");
if (!token) window.location.href = "login.html";

function authHeaders() {
    return { Authorization: "Bearer " + token };
}

document.getElementById("btnBack").addEventListener("click", () => {
    window.location.href = "chat.html";
});

document.getElementById("btnLoad").addEventListener("click", async () => {
    document.getElementById("msg").innerText = "Loading...";
    document.getElementById("list").innerHTML = "";

    try {
        const res = await fetch(`${API_BASE}/api/chat/history`, {
            method: "GET",
            headers: { ...authHeaders() },
        });

        if (!res.ok) {
            const err = await res.text();
            document.getElementById(
                "msg"
            ).innerText = `Error: ${res.status} ${err}`;
            return;
        }

        const items = await res.json(); // array of {Id, Role, Content, CreatedAt}
        document.getElementById(
            "msg"
        ).innerText = `Loaded ${items.length} messages`;

        for (const m of items) {
            const div = document.createElement("div");
            div.innerHTML = `
            <hr>
            <b>${m.Role}</b> — <small>${m.CreatedAt}</small><br>
            ${escapeHtml(m.Content)}
        `;
            document.getElementById("list").appendChild(div);
        }
    } catch (e) {
        document.getElementById("msg").innerText =
            "Network / CORS error: " + e.message;
    }
});

function escapeHtml(text) {
    return (text || "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;");
}