const API_BASE = "https://localhost:44325";

const token = localStorage.getItem("token");
const username = localStorage.getItem("username");

if (!token) window.location.href = "login.html";
document.getElementById("who").innerText = username || "(unknown)";

function authHeaders() {
    return { Authorization: "Bearer " + token };
}

document.getElementById("btnSend").addEventListener("click", async () => {
    const msg = document.getElementById("question").value.trim();
    if (!msg) return;

    document.getElementById("replyBox").innerText = "Loading...";

    try {
        const res = await fetch(`${API_BASE}/api/chat/send`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                ...authHeaders(),
            },
            body: JSON.stringify({ Message: msg }),
        });

        if (!res.ok) {
            const err = await res.text();
            document.getElementById(
                "replyBox"
            ).innerText = `Error: ${res.status} ${err}`;
            return;
        }

        const data = await res.json(); // { Id, Role, Content, CreatedAt }
        document.getElementById("replyBox").innerText = data.Content;
        document.getElementById("question").value = "";
    } catch (e) {
        document.getElementById("replyBox").innerText =
            "Network / CORS error: " + e.message;
    }
});

document.getElementById("btnHistory").addEventListener("click", () => {
    window.location.href = "history.html";
});

document
    .getElementById("btnLogout")
    .addEventListener("click", async () => {
        try {
            await fetch(`${API_BASE}/api/logout`, {
                method: "POST",
                headers: { ...authHeaders() },
            });
        } catch {
            /* ignore */
        }

        localStorage.removeItem("token");
        localStorage.removeItem("username");
        window.location.href = "login.html";
    });