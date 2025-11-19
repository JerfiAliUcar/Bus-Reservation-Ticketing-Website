$(document).ready(function () {

    /* --- KOLTUK SEÇİM MANTIĞI (Sadece Seat-Selection sayfasında çalışır) --- */
    if ($('.seat').length) {
        const seatPrice = 500; // Bilet Fiyatı
        let selectedSeats = [];

        // Koltuğa tıklama olayı
        $('.seat').on('click', function () {
            // Doluysa işlem yapma
            if ($(this).hasClass('sold')) {
                alert('Bu koltuk dolu!');
                return;
            }

            const seatNum = $(this).data('id');

            // Zaten seçiliyse çıkar
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
                selectedSeats = selectedSeats.filter(id => id !== seatNum);
            } else {
                // Maksimum 5 koltuk sınırı
                if (selectedSeats.length >= 5) {
                    alert('Tek seferde en fazla 5 koltuk seçebilirsiniz.');
                    return;
                }
                // Yeni seçimi ekle
                $(this).addClass('selected');
                selectedSeats.push(seatNum);
            }

            // Özeti güncelle
            updateSeatSummary(selectedSeats, seatPrice);
        });
    }

    /* --- FONKSİYONLAR --- */
    function updateSeatSummary(seats, price) {
        const listElement = $('#selected-seats-list');
        const totalElement = $('#total-price');
        const checkoutBtn = $('#checkout-btn');

        listElement.empty();

        if (seats.length === 0) {
            listElement.append('<li class="list-group-item text-muted small pl-0">Henüz seçim yapılmadı.</li>');
            totalElement.text('0 ₺');
            checkoutBtn.addClass('disabled').prop('disabled', true);
        } else {
            // Numaraları sırala ve listele
            seats.sort((a, b) => a - b).forEach(num => {
                listElement.append(`<li class="list-group-item d-flex justify-content-between pl-0">
                    <span>Koltuk ${num}</span> <span>${price} ₺</span>
                </li>`);
            });

            // Toplamı yaz
            totalElement.text((seats.length * price) + ' ₺');

            // Butonu aktif et
            checkoutBtn.removeClass('disabled').prop('disabled', false);
        }
    }
});