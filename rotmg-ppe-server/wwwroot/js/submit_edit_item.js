$("#submit").click(function () {
    // sum up the values of the checked boxes
    var total = 0;
    $("input:checked").each(function () {
        total += parseInt($(this).val());
    });
    var itemName = $("#itemName").val();
    var itemWorth = $("#itemWorth").val();
    var originalName = $("#originalName").val();
    // build PUT query string to /api/Item/<originalName>
    var json = {
        "name": itemName,
        "worth": itemWorth,
        "itemType": total
    };
    $.ajax({
        url: "/api/Item/" + originalName,
        type: "PUT",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            alert("Item updated successfully!");
            window.location.href = "/api/Item";
        },
        error: function (data) {
            alert("Error updating item!");
        }
    });
}
    


    
    
    