let currentImageIndex = 0;
const images = [
  "images/foto1.jpg",
  "images/foto2.jpg",
  // "images/foto3.jpg"  // Yeni eklenen fotoğrafı buraya eklemeyin, çünkü zaten HTML dosyasında var
];

const galleryImg1 = document.getElementById("galleryImg1");
const galleryImg2 = document.getElementById("galleryImg2");

function prevImage() {
  currentImageIndex = (currentImageIndex - 1 + images.length) % images.length;
  if (currentImageIndex === 0) {
    galleryImg1.style.animation = "slideFromTop 0.5s ease"; // Yukarıdan aşağıya kaydırma animasyonu
    galleryImg1.style.display = "block";
    galleryImg2.style.display = "none";
  } else {
    galleryImg2.style.animation = "slideFromTop 0.5s ease"; // Yukarıdan aşağıya kaydırma animasyonu
    galleryImg1.style.display = "none";
    galleryImg2.style.display = "block";
  }
}

function nextImage() {
  currentImageIndex = (currentImageIndex + 1) % images.length;
  if (currentImageIndex === 0) {
    galleryImg1.style.animation = "slideFromTop 0.5s ease"; // Yukarıdan aşağıya kaydırma animasyonu
    galleryImg1.style.display = "block";
    galleryImg2.style.display = "none";
  } else {
    galleryImg2.style.animation = "slideFromTop 0.5s ease"; // Yukarıdan aşağıya kaydırma animasyonu
    galleryImg1.style.display = "none";
    galleryImg2.style.display = "block";
  }
}


let count1 = document.querySelector(".Happy h1");
let count2 = document.querySelector(".Medical h1");
let count3 = document.querySelector(".Doctors1 h1");
let count4 = document.querySelector(".Experience h1");



function animateValue(item, start, end, duration) {
  if (start === end) return;
  var range = end - start;
  var current = start;
  var increment = end > start? 10 : -1;
  var stepTime = Math.abs(Math.floor(duration / range));
  var obj = item
  var timer = setInterval(function() {
      current += increment;
      obj.innerHTML = current;
      if (current == end) {
          clearInterval(timer);
      }
  }, stepTime);
}


animateValue(count1, 0, 1250, 1000);
animateValue(count2, 0, 1250, 1000);
animateValue(count3, 0, 1250, 1000);
animateValue(count4, 0, 1250, 1000);



