const API_BASE = "https://localhost:44325"; // your API base

document
    .getElementById("btnLogin")
    .addEventListener("click", async () => {
        const userName = document.getElementById("username").value.trim();
        const password = document.getElementById("password").value;

        if (!userName || !password) {
            document.getElementById("msg").innerText =
                "Username + Password required";
            return;
        }

        try {
            const res = await fetch(`${API_BASE}/api/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ UserName: userName, Password: password }),
            });

            if (!res.ok) {
                const err = await res.text();
                document.getElementById(
                    "msg"
                ).innerText = `Login failed: ${res.status} ${err}`;
                return;
            }

            const tokenDto = await res.json(); // { TKey, CreatedAt, ExpireAt, UserName }
            localStorage.setItem("token", tokenDto.TKey);
            localStorage.setItem("username", tokenDto.UserName);

            window.location.href = "chat.html";
        } catch (e) {
            document.getElementById("msg").innerText =
                "Network / CORS error: " + e.message;
        }
    });