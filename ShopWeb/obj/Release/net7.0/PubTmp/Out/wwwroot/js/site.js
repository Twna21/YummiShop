
$(() => {
    LoadProdData();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();

    connection.start();
    connection.on("LoadProducts", function () {
        LoadProdData();
    })

    LoadProdData();
    function LoadProdData() {
        var tr = '';
        $.ajax({
            url: '/Admin/getproduct',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td>${v.productId}</td>
                        <td>${v.categoryId}</td>
                        <td>${v.productName}</td>
                        <td><img width="150px" src="${v.productImage}" alt="${v.productName}"></td>
                        <td>${v.unitPrice}</td>
                        <td>${v.quantityPerUnit}</td>
                        <td>
                            <a href="javascript:void(0);" onclick="deleteProduct(${v.productId})" class="delete">
                                <span class="material-symbols-outlined" title="delete">delete</span>
                            </a>
                        </td>
                    </tr>`;
                })
                $("#table12").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
})
