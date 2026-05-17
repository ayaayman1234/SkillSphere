/* =========================
   SkillSphere JavaScript
   ========================= */

/* 1. FORM VALIDATION (Create Skill) */
function validateForm() {

    let title = document.getElementById("Title")?.value;

    let description = document.getElementById("Description")?.value;

    let offer = document.getElementById("Offer")?.value;

    if (!title || title.trim() === "") {
        alert("Title is required");
        return false;
    }

    if (!description || description.length < 10) {
        alert("Description must be at least 10 characters");
        return false;
    }

    if (!offer || offer.trim() === "") {
        alert("Offer is required");
        return false;
    }

    return true;
}


/* 2. AJAX DELETE (WOW FEATURE 🔥) */
function deleteSkill(id) {

    fetch('/SkillPosts/Delete/' + id, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                document.getElementById("skill-" + id).remove();
                alert("Skill deleted successfully");
            } else {
                alert("Error deleting skill");
            }
        });

}


/* 3. REQUEST SWAP (Fake for now) */
function requestSwap(id) {

    alert("Swap request sent successfully for Skill ID: " + id);

}


/* 4. TOAST (Optional upgrade - future use) */
function showMessage(message) {
    alert(message);
}