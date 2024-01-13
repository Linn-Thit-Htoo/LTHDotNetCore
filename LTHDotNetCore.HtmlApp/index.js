const tblName = "Tbl_Name";
var _editId = null;

$("#nameErr").css('display', 'none');

$("#btnSave").click(function () {
    _editId == null ? createData() : updateData();
});

function createData() {
    const name = $("#name").val();

    if (name == null || name == "") {
        $("#nameErr").css('display', 'block');
        return;
    }

    let lst = [];

    const data = {
        Id: uuidv4(),
        Name: name
    };

    if (localStorage.getItem(tblName) != null) {
        lst = JSON.parse(localStorage.getItem(tblName));
    }

    lst.push(data);

    localStorage.setItem(tblName, JSON.stringify(lst));
    $("#nameErr").css('display', 'none');
    $("#name").val("");

    successMessage("Saving Successful.");

    readData();
}

function readData() {
    if (localStorage.getItem(tblName) == null) return;

    var lst = JSON.parse(localStorage.getItem(tblName));
    var rows = "";
    var count = 0;

    lst.forEach(item => {
        rows += `<tr>
        <th>${++count}</th>
        <td>${item.Id}</td>
        <td>${item.Name}</td>
        <td>
        <button type="button" class="btn btn-warning" onclick="editData('${item.Id}')">
            <i class="fa-solid fa-pen-to-square"></i>
        </button>
        <button type="button" class="btn btn-danger" style="margin-left:2%" onclick="deleteData('${item.Id}')">
            <i class="fa-solid fa-trash"></i>
        </button>
    </td>
      </tr>`
    });
    $("#tbody").html(rows);
}

function editData(id) {
    var lst = JSON.parse(localStorage.getItem(tblName));
    var results = lst.filter(x => x.Id == id);
    if (results.length == 0) {
        alert('No data found.')
        return;
    }
    var item = results[0];
    _editId = item.Id;
    $("#name").val(item.Name);
}

function updateData() {
    let name = $("#name").val();
    if (name == "" || name == null) {
        $("#nameErr").css('display', 'block');
        return;
    }
    var lst = JSON.parse(localStorage.getItem(tblName));
    let index = lst.findIndex(x => x.Id == _editId);
    lst[index].Name = $("#name").val();
    localStorage.setItem(tblName, JSON.stringify(lst));
    $("#name").val("");
    $("#nameErr").css('display', 'none');

    successMessage("Updating Successful.");

    readData();
}

function deleteData(id) {
    confirmMessage("Are you sure to delete?").then(res => {
        if (!res) return;

        var lst = JSON.parse(localStorage.getItem(tblName));
        var results = lst.filter(x => x.Id !== id);
        if (results.length == 0) {
            alert("No data found.")
            return;
        }
        localStorage.setItem(tblName, JSON.stringify(results));
        readData();
    }).catch(err => {
        console.warn(err);
    });
}

readData();